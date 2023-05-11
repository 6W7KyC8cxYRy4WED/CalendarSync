using CalendarSync.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CalendarSync.MicrosoftGraph
{
    public static class MicrosoftManager
    {
        private static GraphServiceClient? GetGraphServiceClient(string microsoftUserEmail, DatabaseContext databaseContext)
        {
            MicrosoftUser microsoftUser = databaseContext.MicrosoftUsers.SingleOrDefault(mu => mu.Email == microsoftUserEmail)!;

            if(microsoftUser != null)
            {
                AccessTokenStatus accessTokenStatus = AccessTokenAgent.Status(microsoftUser.AccessToken);

                //if (accessTokenStatus == AccessTokenStatus.Error)
                //{
                //    microsoftUser.LastAccessTokenStatus = (int)AccessTokenStatus.Error;
                //    databaseContext.SaveChanges();
                //    return null;
                //}

                if (accessTokenStatus == AccessTokenStatus.Expired || accessTokenStatus == AccessTokenStatus.Error)
                {
                    AccessToken accessToken = AccessTokenAgent.Refresh(microsoftUser.RefreshToken);

                    if (accessToken.Error)
                    {
                        microsoftUser.LastAccessTokenStatus = (int)AccessTokenStatus.Error;
                        databaseContext.SaveChanges();
                        return GetGraphServiceClient(microsoftUserEmail, databaseContext);
                    }

                    else
                    {
                        microsoftUser.AccessToken = accessToken.Access;
                        microsoftUser.RefreshToken = accessToken.Refresh;
                        microsoftUser.LastAccessTokenStatus = (int)AccessTokenStatus.Expired;
                        databaseContext.SaveChanges();
                        return GetGraphServiceClient(microsoftUserEmail, databaseContext);
                    }
                }

                else if (accessTokenStatus == AccessTokenStatus.Valid)
                {
                    microsoftUser.LastAccessTokenStatus = (int)AccessTokenStatus.Valid;
                    databaseContext.SaveChanges();
                    return GetGraphServiceClient(microsoftUser.AccessToken);
                }

                else
                {
                    return null;
                }
            }

            else
            {
                return null;
            }
        }

        public static void Sync(DatabaseContext databaseContext)
        {
            List<MicrosoftUser> microsoftUsers = databaseContext.MicrosoftUsers.Include(mu => mu.Calendars).Where(mu => mu.LastAccessTokenStatus != (int)AccessTokenStatus.Error).ToList();

            foreach (MicrosoftUser microsoftUser in microsoftUsers)
            {
                GraphServiceClient graphServiceClientOriginUser = GetGraphServiceClient(microsoftUser.Email, databaseContext);

                foreach (Entities.Calendar calendar in microsoftUser.Calendars)
                {
                    CalendarEventsCollectionPage? calendarEvents = null;

                    do
                    {
                        if (calendarEvents == null)
                        {
                            string startTime = "start/dateTime ge '" + DateTime.Now.ToString("yyyy-MM-dd") + "T00:00:00Z'";
                            calendarEvents = (CalendarEventsCollectionPage)graphServiceClientOriginUser.Me.Calendars[calendar.Id].Events.Request().Filter(startTime).Top(999).GetAsync().Result;
                        }

                        else
                        {
                            calendarEvents = (CalendarEventsCollectionPage)calendarEvents.NextPageRequest.GetAsync().Result;
                        }

                        foreach (var @event in calendarEvents)
                        {
                            if (!databaseContext.Events.Any(e => e.Id == @event.Id) && !databaseContext.ClonedEvents.Any(ce => ce.Id == @event.Id))
                            {
                                Entities.Event newLocalEvent = new Entities.Event();
                                newLocalEvent.RootCalendar = calendar;
                                newLocalEvent.Id = @event.Id;
                                newLocalEvent.ChangeKey = @event.ChangeKey;
                                databaseContext.Add(newLocalEvent);
                                List<LinkedCalendar> linkedCalendars = databaseContext.LinkedCalendars.Where(lc => lc.RootCalendarId == calendar.Id).ToList();

                                foreach (LinkedCalendar linkedCalendar in linkedCalendars)
                                {
                                    Microsoft.Graph.Event newGraphEvent = CloneMicrosofotEvent(@event);
                                    string linkedMicrosoftUserEmail = databaseContext.Calendars.Single(c => c.Id == linkedCalendar.LinkedCalendarId).MicrosoftUserEmail;
                                    MicrosoftUser targetMicrosoftUser = microsoftUsers.Single(mu => mu.Email == linkedMicrosoftUserEmail);
                                    GraphServiceClient graphServiceClientTargetUser = GetGraphServiceClient(targetMicrosoftUser.Email, databaseContext);
                                    Microsoft.Graph.Event pushedEvent = graphServiceClientTargetUser.Me.Calendars[linkedCalendar.LinkedCalendarId].Events.Request().AddAsync(newGraphEvent).Result;
                                    ClonedEvent clonedEvent = new ClonedEvent();
                                    clonedEvent.Id = pushedEvent.Id;
                                    clonedEvent.RootCalendarId = linkedCalendar.LinkedCalendarId;
                                    clonedEvent.ChangeKey = pushedEvent.ChangeKey;
                                    newLocalEvent.ClonedEvents.Add(clonedEvent);
                                    databaseContext.SaveChanges(); 
                                }
                            }

                            else if (databaseContext.Events.Any(e => e.Id == @event.Id && e.ChangeKey != @event.ChangeKey))
                            {
                                Entities.Event updatedEvent = databaseContext.Events.Include(e => e.ClonedEvents).ThenInclude(ce => ce.RootCalendar).ThenInclude(rc => rc.MicrosoftUser).Single(e => e.Id == @event.Id);
                                updatedEvent.ChangeKey = @event.ChangeKey;
                                databaseContext.SaveChanges();

                                foreach (ClonedEvent clonedEvent in updatedEvent.ClonedEvents)
                                {
                                    GraphServiceClient graphServiceClientTargetUser = GetGraphServiceClient(clonedEvent.RootCalendar.MicrosoftUser.Email, databaseContext);
                                    Microsoft.Graph.Event remoteEvent = graphServiceClientTargetUser.Me.Calendars[clonedEvent.RootCalendar.Id].Events[clonedEvent.Id].Request().GetAsync().Result;
                                    UpdateMicrosoftEvent(remoteEvent, @event);
                                    remoteEvent = graphServiceClientTargetUser.Me.Calendars[clonedEvent.RootCalendar.Id].Events[clonedEvent.Id].Request().UpdateAsync(remoteEvent).Result;
                                }    
                            }
                        }

                    } while (calendarEvents.NextPageRequest != null);
                }
            }
        }

        private static GraphServiceClient GetGraphServiceClient(string accessToken)
        {
            DelegateAuthenticationProvider authProvider = new DelegateAuthenticationProvider((request) =>
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return Task.CompletedTask;
            });

            return new GraphServiceClient(authProvider);
        }

        private static Microsoft.Graph.Event CloneMicrosofotEvent(Microsoft.Graph.Event @event)
        {
            Microsoft.Graph.Event newEvent = new Microsoft.Graph.Event();
            return UpdateMicrosoftEvent(newEvent, @event);
        }

        private static Microsoft.Graph.Event UpdateMicrosoftEvent(Microsoft.Graph.Event clonedEvent, Microsoft.Graph.Event originEvent)
        {
            Microsoft.Graph.Event newGraphEvent = new Microsoft.Graph.Event();
            clonedEvent.Subject = originEvent.Subject;
            clonedEvent.Body = originEvent.Body;
            clonedEvent.IsAllDay = originEvent.IsAllDay;
            clonedEvent.Start = originEvent.Start;
            clonedEvent.End = originEvent.End;
            clonedEvent.IsCancelled = originEvent.IsCancelled;
            clonedEvent.IsReminderOn = false;
            return clonedEvent;
        }
    }
}

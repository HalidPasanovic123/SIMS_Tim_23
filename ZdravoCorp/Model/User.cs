/***********************************************************************
 * Module:  User.cs
 * Author:  halid
 * Purpose: Definition of the Class Model.User
 ***********************************************************************/

using System;

namespace Model
{
   public abstract class User
   {
      protected String password;
      protected String username;
      protected String name;
      protected String surname;
      protected String email;
      protected String address;
      protected String id;
      protected String phoneNumber;
      protected Gender gender;
      protected DateTime dateOfBirth;
      
      protected System.Collections.ArrayList notifications;
      
      /// <summary>
      /// Property for collection of Notification
      /// </summary>
      /// <pdGenerated>Default opposite class collection property</pdGenerated>
      public System.Collections.ArrayList Notifications
      {
         get
         {
            if (notifications == null)
               notifications = new System.Collections.ArrayList();
            return notifications;
         }
         set
         {
            RemoveAllNotifications();
            if (value != null)
            {
               foreach (Notification oNotification in value)
                  AddNotifications(oNotification);
            }
         }
      }
      
      /// <summary>
      /// Add a new Notification in the collection
      /// </summary>
      /// <pdGenerated>Default Add</pdGenerated>
      public void AddNotifications(Notification newNotification)
      {
         if (newNotification == null)
            return;
         if (this.notifications == null)
            this.notifications = new System.Collections.ArrayList();
         if (!this.notifications.Contains(newNotification))
            this.notifications.Add(newNotification);
      }
      
      /// <summary>
      /// Remove an existing Notification from the collection
      /// </summary>
      /// <pdGenerated>Default Remove</pdGenerated>
      public void RemoveNotifications(Notification oldNotification)
      {
         if (oldNotification == null)
            return;
         if (this.notifications != null)
            if (this.notifications.Contains(oldNotification))
               this.notifications.Remove(oldNotification);
      }
      
      /// <summary>
      /// Remove all instances of Notification from the collection
      /// </summary>
      /// <pdGenerated>Default removeAll</pdGenerated>
      public void RemoveAllNotifications()
      {
         if (notifications != null)
            notifications.Clear();
      }
   
   }
}
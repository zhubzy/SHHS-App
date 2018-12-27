﻿using System;
using System.Threading;
using Plugin.LocalNotifications.Abstractions;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace Plugin.LocalNotifications
{
    /// <summary>
    /// Access Cross Local Notifictions
    /// </summary>
    public static class CrossLocalNotifications
    {
        private static Lazy<ILocalNotifications> _impl = new Lazy<ILocalNotifications>(CreateLocalNotificationsImplementation, LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets the current platform specific ILocalNotifications implementation.
        /// </summary>
        public static ILocalNotifications Current
        {
            get
            {
                var val = _impl.Value;
                if (val == null)
                    throw NotImplementedInReferenceAssembly();
                return val;
            }
        }

        private static ILocalNotifications CreateLocalNotificationsImplementation()
        {
#if NETSTANDARD1_0
            return null;
#else

            return DependencyService.Get<ILocalNotifications>();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException();
        }
    }
}

using Microsoft.Phone.Info;

namespace Phone7.Fx
{
    public static class ExtendedPropertyHelper
    {
        private static readonly int ANIDLength = 32;
        private static readonly int ANIDOffset = 2;

        public static string GetManufacturer()
        {
            string result = string.Empty;
            object manufacturer;
            if (DeviceExtendedProperties.TryGetValue("DeviceManufacturer", out manufacturer))
                result = manufacturer.ToString();

            return result;
        }

        //Note: to get a result requires ID_CAP_IDENTITY_DEVICE  
        /// <summary>
        /// Gets the device unique ID.
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceUniqueID()
        {
            string result = null;
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                result = System.Convert.ToBase64String((byte[])uniqueId);

            return result;
        }

        // NOTE: to get a result requires ID_CAP_IDENTITY_USER  
        /// <summary>
        /// Gets the windows live anonymous ID.
        /// </summary>
        /// <returns></returns>
        public static string GetWindowsLiveAnonymousID()
        {
            string result = string.Empty;
            object anid;
            if (UserExtendedProperties.TryGetValue("ANID", out anid))
            {
                if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))
                {
                    result = anid.ToString().Substring(ANIDOffset, ANIDLength);
                }
            }

            return result;
        }
    }
}
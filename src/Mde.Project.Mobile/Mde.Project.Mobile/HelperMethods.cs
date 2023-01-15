using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms;

namespace Mde.Project.Mobile
{
    public static class HelperMethods
    {
        public static bool CheckOs()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

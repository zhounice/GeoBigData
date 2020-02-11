using DistributedGIS.Utils;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedGIS.ESRI.Extensions
{
    public class LicenseUtil
    {
        private static AdvAELicenseClass m_AdvAELicenseClass = new AdvAELicenseClass();

        public static bool CheckOutLicenseAdvanced()
        {
            return m_AdvAELicenseClass.CheckOutLicenseAdvanced();
        }

        public static void ShutDown()
        {
            m_AdvAELicenseClass.ShutDown();
        }
    }

    public  class AdvAELicenseClass
    {
        private IAoInitialize m_AoInitializeClass;

        public AdvAELicenseClass()
        {
          RuntimeManager.Bind(ProductCode.EngineOrDesktop);
            m_AoInitializeClass = new AoInitializeClass();
        }

        public bool CheckOutLicenseAdvanced()
        {
            bool result = CheckOutLicenseMain(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
            if (!result)
            {
                result = CheckOutLicenseMain(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            }
            if (!result)
            {
                result = CheckOutLicenseMain(esriLicenseProductCode.esriLicenseProductCodeStandard);
            }
            if (!result)
            {
                result = CheckOutLicenseMain(esriLicenseProductCode.esriLicenseProductCodeEngine);
            }
            if (!result)
            {
                result = CheckOutLicenseMain(esriLicenseProductCode.esriLicenseProductCodeArcServer);
            }
            if (!result)
            {
                result = CheckOutLicenseMain(esriLicenseProductCode.esriLicenseProductCodeBasic);
            }

            return result;
        }

        //签出产品许可
        private bool CheckOutLicenseMain(esriLicenseProductCode code)
        {
            try
            {
                if (m_AoInitializeClass.IsProductCodeAvailable(code) == esriLicenseStatus.esriLicenseAvailable)
                {
                    if (m_AoInitializeClass.Initialize(code) == esriLicenseStatus.esriLicenseCheckedOut)
                    {
                        m_AoInitializeClass.InitializedProduct();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }

            return false;
        }

        public void ShutDown()
        {
            m_AoInitializeClass.Shutdown();
        }
    }
}

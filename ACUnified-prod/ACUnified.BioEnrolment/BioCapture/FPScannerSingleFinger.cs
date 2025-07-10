using Dermalog.Afis.FingerCode3;
using Dermalog.Afis.NistQualityCheck;
using Dermalog.Imaging.Capturing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.BioEnrol.BioCapture
{
    public abstract class FPScannerSingleFinger : FPScanner
    {
        public FPScannerSingleFinger(DeviceIdentity deviceIdentity, int index, CaptureMode captureMode)
            : base(deviceIdentity, index, captureMode)
        {
        }

        public void setLeds(bool enable)
        {
            setGreenLed(enable);
            setRedLed(enable);
        }

        #region SingleFinger-Scanner specific functions to implement
        public abstract void setGreenLed(bool enable);
        public abstract void setRedLed(bool enable);
        #endregion

        #region Implementation of abstract methods in base-class
        public override void StartCapturing()
        {
            base.Start();

            setGreenLed(true);
        }

        public override void StopCapturing()
        {
            base.Stop();

            setLeds(false);
        }

        protected override void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                setGreenLed(false);
                setRedLed(true);

                var localImage = e.Argument as System.Drawing.Image;

                Template t1 = base.Encoder.Encode(localImage);

                int nQC = DermalogNistQualityCheck.Check(localImage);

                Fingerprint fp = new Fingerprint();
                fp.Image = localImage;
                fp.Template = t1;
                fp.NFIQ = nQC;

                List<Fingerprint> fps = new List<Fingerprint>()
                {
                    fp
                };

                base.InvokeFingerprintsDetected(fps);

                setGreenLed(true);
                setRedLed(false);
            }
            catch (Exception ex)
            {
                InvokeScannerError(sender, new ScannerErrorEventArgs("Processing error: " + ex.Message, ex));
            }
        }
        #endregion
    }
}

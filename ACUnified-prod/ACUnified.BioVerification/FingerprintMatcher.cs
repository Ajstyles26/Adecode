using Dermalog.Afis.FingerCode3;
using Dermalog.Afis.FingerCode3.Enums;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dermalog.Afis.ImageContainer;
namespace ACUnified.BioVerification
{
    public  class FingerprintMatcher
    {
        //private  Dermalog.Afis.FingerCode3.Encoder encoder = new Dermalog.Afis.FingerCode3.Encoder();
        //private  Dermalog.Afis.FingerCode3.Matcher matcher = new Dermalog.Afis.FingerCode3.Matcher();
        //private  Dermalog.Afis.FingerCode3.Template initialTemplate, currentTemplate;
        public  void MatchFingerPrint(string initialSource, string currentSource)
        {
            Bitmap initbitmap = new Bitmap(initialSource);
            Bitmap currentbitmap = new Bitmap(currentSource);
            
          var initialTemplate= new Dermalog.Afis.FingerCode3.Encoder().Encode(initbitmap);
           var currentTemplate = new Dermalog.Afis.FingerCode3.Encoder().Encode(initbitmap);
           var result=new Matcher().Match(initialTemplate, currentTemplate);
        }
    }
}

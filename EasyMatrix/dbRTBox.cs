using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DataboundRTB
{
    [ClassInterface(ClassInterfaceType.AutoDispatch), DefaultBindingProperty("Rtf"), Description("DescriptionRichTextBox"), ComVisible(true), Docking(DockingBehavior.Ask), Designer("System.Windows.Forms.Design.RichTextBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    class dbRTBox : System.Windows.Forms.RichTextBox
    {

        [Bindable(true), RefreshProperties(RefreshProperties.All), SettingsBindable(true), DefaultValue(false), Category("Appearance")]
        new public string Rtf
        {
            get
            {
                return base.Rtf;
            }
            set
            {
                base.Rtf = value;
            }
        }

    }
}

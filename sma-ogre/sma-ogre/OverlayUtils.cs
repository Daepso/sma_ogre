using Mogre;
using System;
using sma_ogre.utils;

namespace sma_ogre
{
    class OverlayNotFoundException : Exception {}

    class OverlayUtils
    {
        static private OverlayUtils singleton;

        private OverlayUtils()
        {
            CreateOverlaysTemplate();
            CreateHelpOverlay();
            CreateInformationOverlay();
        }

        public static OverlayUtils Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new OverlayUtils();
                }
                return singleton;
            }
        }

        private void CreateOverlaysTemplate()
        {
            var msgBoxTpl = (PanelOverlayElement)OverlayManager.Singleton.CreateOverlayElement("Panel", "Templates/MessageBox", true);
            msgBoxTpl.MaterialName = "Core/StatsBlockCenter";
            msgBoxTpl.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            msgBoxTpl.Width = 250;
            msgBoxTpl.Height = 150;

            var text = (TextAreaOverlayElement)OverlayManager.Singleton.CreateOverlayElement("TextArea", "Templates/MessageBox/Body", true);
            text.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            text.Left = 10;
            text.Top = 20;
            text.FontName = "BlueHighway";
            text.CharHeight = 16;
            text.SetAlignment(TextAreaOverlayElement.Alignment.Left);
            text.Colour = new ColourValue(0.5f, 0.7f, 0.5f);
            msgBoxTpl.AddChild(text);

            var title = (TextAreaOverlayElement)OverlayManager.Singleton.CreateOverlayElement("TextArea", "Templates/MessageBox/Title", true);
            title.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            title.FontName = "BlueHighway";
            title.Left = msgBoxTpl.Width/2;
            title.CharHeight = 20;
            title.SetAlignment(TextAreaOverlayElement.Alignment.Center);
            title.Colour = new ColourValue(0.5f, 0.7f, 0.5f);
            msgBoxTpl.AddChild(title);
        }

        private void CreateHelpOverlay()
        {
            BorderPanelOverlayElement helpBox = (BorderPanelOverlayElement)OverlayManager.Singleton.CreateOverlayElementFromTemplate(
                "Templates/MessageBox", "BorderPanel", "HelpBox");
            helpBox.Left = 10;
            helpBox.Top = 20;
            helpBox.Width = 300;
            helpBox.Height = 300;
            OverlayManager.Singleton.Create("HelpOverlay").Add2D(helpBox);
        }

        private void CreateInformationOverlay()
        {
            BorderPanelOverlayElement infoBox = (BorderPanelOverlayElement)OverlayManager.Singleton.CreateOverlayElementFromTemplate(
                "Templates/MessageBox", "BorderPanel", "InfoBox");
            infoBox.Left = 300;
            infoBox.Top = 20;
            infoBox.Width = 300;
            infoBox.Height = 300;
            OverlayManager.Singleton.Create("InformationOverlay").Add2D(infoBox);
        }

        private void ToggleOverlay(Overlay overlay)
        {
            if (overlay.IsVisible)
                overlay.Hide();
            else
                overlay.Show();
        }

        public void ToggleHelp()
        {
            var helpOverlay = OverlayManager.Singleton.GetByName("HelpOverlay");
            ToggleOverlay(helpOverlay);
        }

        public void ToggleInformation()
        {
            var informationOverlay = OverlayManager.Singleton.GetByName("InformationOverlay");
            ToggleOverlay(informationOverlay);
        }

        public void Update()
        {
            UpdateHelp();
            UpdateInformation();
        }

        private void UpdateHelp()
        {
            OverlayManager.Singleton.GetOverlayElement("HelpBox" + "/Templates/MessageBox/Title").Caption = "Help";
            OverlayManager.Singleton.GetOverlayElement("HelpBox" + "/Templates/MessageBox/Body").Caption =
                "h : display this help\n" +
                "i : display informations about the world\n" +
                "l : toggle ambien light\n" +
                "\n" +
                "P : Pause\n" +
                "p : increase life speed\n" +
                "m : decrease life speed\n" +
                "\n" +
                "Move mouse for camera rotation\n" +
                "a : move camera upward\n" +
                "e : move camera downward\n" +
                "z, up : move camera forward\n" +
                "s, down : move camera backward\n" +
                "q, left : move camera left\n" +
                "d, right : move camera right\n" +
                "(when shift key is pressed camera moves faster)\n" ;
        }

        private void UpdateInformation()
        {
            OverlayManager.Singleton.GetOverlayElement("InfoBox" + "/Templates/MessageBox/Title").Caption = "Information";
            OverlayManager.Singleton.GetOverlayElement("InfoBox" + "/Templates/MessageBox/Body").Caption =
                "Time: "                   + WorldTime.Singleton.GetTimeString()      + " s\n" +
                "Number of robots: "       + InformationLogger.Singleton.RobotsNumber +   "\n" +
                "Number of ogres: "        + InformationLogger.Singleton.OgresNumber  +   "\n" +
                "Birth since beginning: "  +                                       "" +   "\n" +
                "Death since beginning: "  +                                       "" +   "\n" +
                "Life acceleration rate: " +                                       "" +   "\n" ;
        }
    }
}

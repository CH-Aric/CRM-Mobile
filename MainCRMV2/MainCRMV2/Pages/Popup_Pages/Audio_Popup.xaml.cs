using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Popup_Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Audio_Popup : PopupPage
    {
        ISimpleAudioPlayer player;
        public Audio_Popup(string audiofile)
        {
            InitializeComponent();
            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(audiofile);
            player.Loop = true;
            this.BackgroundColor = Color.WhiteSmoke;
        }
        public void onClickedPlay(object sender, EventArgs e)
        {
            if (player.IsPlaying)
            {
                playButton.Text = "Play";
                player.Pause();
            }
            else
            {
                playButton.Text = "Pause";
                player.Play();
            }
        }
    }
}
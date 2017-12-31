using System;
namespace mvvmframework.Models
{
    public class UIAttributes
    {
        public string Text { get; set; }
        public bool ScreenLeft { get; set; } = true; // left
        public int Position { get; set; } // 0 = edge of screen irrespective of side
        public bool TextBold { get; set; } = false; // not bold
        public bool IsImageSource { get; set; } = false; // not image
        public EventHandler ClickEvent { get; set; } = null; // no clickevent
    }
}

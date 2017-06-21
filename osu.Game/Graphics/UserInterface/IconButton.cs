﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;

namespace osu.Game.Graphics.UserInterface
{
    public class IconButton : ClickableContainer
    {
        private readonly TextAwesome icon;
        private readonly Box hover;
        private readonly Container content;

        public FontAwesome Icon
        {
            get { return icon.Icon; }
            set { icon.Icon = value; }
        }

        private const float button_size = 30;
        private Color4 flashColour;

        public Vector2 IconScale
        {
            get { return icon.Scale; }
            set { icon.Scale = value; }
        }

        private Color4 disableColour;

        public IconButton()
        {
            AutoSizeAxes = Axes.Both;

            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;

            Children = new Drawable[]
            {
                content = new Container
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Size = new Vector2 (button_size),

                    CornerRadius = 5,
                    Masking = true,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = Color4.Black.Opacity(0.04f),
                        Type = EdgeEffectType.Shadow,
                        Radius = 5,
                    },
                    Children = new Drawable[]
                    {
                        hover = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0,
                        },
                        icon = new TextAwesome
                        {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            TextSize = 18,
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            hover.Colour = colours.Yellow.Opacity(0.6f);
            flashColour = colours.Yellow;
            disableColour = colours.Gray9;

            Enabled.ValueChanged += enabledChanged;
        }

        private void enabledChanged(bool newEnabled)
        {
            if (newEnabled)
            {
                FadeColour(Color4.White, 200, EasingTypes.OutQuint);

                if (Hovering)
                    OnHover(new InputState());
            }
            else
            {
                FadeColour(disableColour, 200, EasingTypes.OutQuint);
                content.ScaleTo(1, 200, EasingTypes.OutElastic);

                if (Hovering)
                    OnHoverLost(new InputState());
            }
        }

        protected override bool OnHover(InputState state)
        {
            if (!Enabled.Value)
                return true;

            hover.FadeIn(500, EasingTypes.OutQuint);
            return base.OnHover(state);
        }

        protected override void OnHoverLost(InputState state)
        {
            if (!Enabled.Value)
                return;

            hover.FadeOut(500, EasingTypes.OutQuint);
            base.OnHoverLost(state);
        }

        protected override bool OnClick(InputState state)
        {
            hover.FlashColour(flashColour, 800, EasingTypes.OutQuint);
            return base.OnClick(state);
        }

        protected override bool OnMouseDown(InputState state, MouseDownEventArgs args)
        {
            if (!Enabled.Value)
                return true;

            content.ScaleTo(0.75f, 2000, EasingTypes.OutQuint);
            return base.OnMouseDown(state, args);
        }

        protected override bool OnMouseUp(InputState state, MouseUpEventArgs args)
        {
            if (!Enabled.Value)
                return true;

            content.ScaleTo(1, 1000, EasingTypes.OutElastic);
            return base.OnMouseUp(state, args);
        }
    }
}

﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Video;
using osu.Framework.IO.Network;
using osu.Framework.Testing;
using osu.Framework.Timing;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Framework.Tests.Visual
{
    public class TestCaseVideoSprite : TestCase
    {
        private ManualClock clock;
        private VideoSprite videoSprite;
        private TextFlowContainer timeText;

        public TestCaseVideoSprite()
        {
            loadVideo();
            Add(new SpriteText { Text = "Video is loading... " });
        }

        private async void loadVideo()
        {
            var wr = new WebRequest("https://assets.ppy.sh/media/landing.mp4");
            await wr.PerformAsync();

            Schedule(() =>
            {
                Clear();

                videoSprite = new VideoSprite(wr.ResponseStream);
                Add(videoSprite);
                videoSprite.Loop = false;

                clock = new ManualClock();
                videoSprite.Clock = new FramedClock(clock);

                Add(timeText = new TextFlowContainer { AutoSizeAxes = Axes.Both });

                AddStep("Jump ahead by 10 seconds", () => clock.CurrentTime += 10_000.0);
                AddStep("Jump back by 10 seconds", () => clock.CurrentTime = Math.Max(0, clock.CurrentTime - 10_000.0));
                AddToggleStep("Toggle looping", newState =>
                {
                    videoSprite.Loop = newState;
                    clock.CurrentTime = 0;
                });
            });
        }

        private int currentSecond;
        private int fps;
        private int lastFramesProcessed;

        protected override void Update()
        {
            base.Update();

            if (clock != null)
                clock.CurrentTime += Clock.ElapsedFrameTime;

            if (videoSprite != null)
            {
                var newSecond = (int)(videoSprite.PlaybackPosition / 1000.0);
                if (newSecond != currentSecond)
                {
                    currentSecond = newSecond;
                    fps = videoSprite.FramesProcessed - lastFramesProcessed;
                    lastFramesProcessed = videoSprite.FramesProcessed;
                }

                if (timeText != null)
                    timeText.Text = $"aim time: {videoSprite.PlaybackPosition:N2}\n"
                                    + $"video time: {videoSprite.CurrentFrameTime:N2}\n"
                                    + $"duration: {videoSprite.Duration:N2}\n"
                                    + $"buffered {videoSprite.AvailableFrames}\n"
                                    + $"FPS: {fps}";
            }
        }
    }
}

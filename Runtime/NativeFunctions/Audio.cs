using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;
using NetCoreAudio;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package MediaPackage = new Package("Audio", new
        {
            playFile = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string fileName = ((StringValue)args[0]).Value;

                // Create player
                Player player = new Player();


                // Play audio
                player.Play(fileName);

                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "playFile",
                Parameters =
                {
                    new()
                    {
                        Name = "audioFile",
                        Type = Values.ValueType.String
                    }
                }
            }),

            createPlayer = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Player player = new Player();

                // Create object for interacting
                ObjectValue obj = Helpers.CreateObject(new
                {
                    isPlaying = Helpers.CreateNativeFunction((args, env, epxr) =>
                    {
                        return Helpers.CreateBoolean(player.Playing);
                    }),

                    isPaused = Helpers.CreateNativeFunction((args, env, expr) =>
                    {
                        return Helpers.CreateBoolean(player.Paused);
                    }),

                    pause = Helpers.CreateNativeFunction((args, env, expr) =>
                    {
                        player.Pause();
                        return Helpers.CreateNull();
                    }),

                    play = Helpers.CreateNativeFunction((args, env, expr) =>
                    {
                        player.Play(((StringValue)args[0]).Value);
                        return Helpers.CreateNull();
                    }, options: new()
                    {
                        Name = "play",
                        Parameters =
                        {
                            new()
                            {
                                Name = "fileName",
                                Type = Values.ValueType.String
                            }
                        }
                    }),
                });

                return obj;
            }, options: new()
            {
                Name = "createPlayer"
            })
        });
    }
}

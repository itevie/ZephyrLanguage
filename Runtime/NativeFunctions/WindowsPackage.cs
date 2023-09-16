using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime.Values.Helpers;
using System.Runtime.InteropServices;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class Packages
    {
        // Load all api calls
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        [DllImport("user32.dll")]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        internal static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

        /// <summary>
        /// This package allows interaction with some Win32 API calls.
        /// This can only be imported on the Windows OS
        /// </summary>
        public static NonDefaultPackage WinformsPackage = new("Windows", new
        {
            messageBox = new
            {
                show = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    // Get values
                    RuntimeValue bodyVal = args[0];
                    RuntimeValue titleVal = args[1];
                    RuntimeValue iconVal = args[2];
                    RuntimeValue buttonsVal = args[3];

                    string? body = ((StringValue?)bodyVal ?? null)?.Value ?? "";
                    string? title = ((StringValue?)titleVal ?? null)?.Value ?? "";
                    int icon = (int)(((IntegerValue?)iconVal ?? null)?.Value ?? 0);
                    int buttons = (int)(((IntegerValue?)buttonsVal ?? null)?.Value ?? 0);

                    int result = MessageBox((IntPtr)0, body ?? "", title ?? "", icon + buttons);
                    return Helpers.CreateInteger(result);
                }, options: new()
                {
                    Name = "show",
                    Parameters =
                    {
                        new()
                        {
                            Name = "body",
                            Type = Values.ValueType.String
                        },
                        new()
                        {
                            Name = "title",
                            Type = Values.ValueType.String
                        },
                        new()
                        {
                            Name = "icon",
                            Type = Values.ValueType.Int
                        },
                        new()
                        {
                            Name = "buttons",
                            Type = Values.ValueType.Int
                        }
                    }
                }),

                icons = new
                {
                    error = 16,
                    question = 32,
                    exclamation = 48,
                    information = 64,
                },

                buttons = new
                {
                    ok = 0,
                    okCancel = 1,
                    abortRetryIgnore = 2,
                    yesNoCancel = 3,
                    yesNo = 4,
                    retryCancel = 5,
                },

                results = new
                {
                    ok = 1,
                    cancel = 2,
                    abort = 3,
                    retry = 4,
                    ignore = 5,
                    yes = 6,
                    no = 7,
                }
            },

            clipboard = new
            {
                set = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    Util.ExpectExact(args, new() { Values.ValueType.String });
                    string val = ((StringValue)args[0]).Value;
                    IntPtr ptr = Marshal.StringToHGlobalUni(val);

                    OpenClipboard(IntPtr.Zero);
                    SetClipboardData(13, ptr);
                    CloseClipboard();
                    Marshal.FreeHGlobal(ptr);

                    return Helpers.CreateNull();
                }, "set")
            }
        }, () =>
        {
            // Check permissions
            if (Program.Options.CanUseSystemAPIs == false)
            {
                return $"This package cannot be used as \"CanUserSystemAPIs\" is denied.\nIf you'd like to use it, run this program with --os-apis=true";
            }

            // Check if running on windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) != true)
            {
                return $"This is a Windows-only package, but got {System.Environment.OSVersion}";
            }

            return null;
        });
    }
}

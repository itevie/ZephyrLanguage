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

        public static NonDefaultPackage WinformsPackage = new NonDefaultPackage("windows", new
        {
            messageBox = new
            {
                show = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    // Get values
                    RuntimeValue? bodyVal = args.ElementAtOrDefault(0);
                    RuntimeValue? titleVal = args.ElementAtOrDefault(1);
                    RuntimeValue? iconVal = args.ElementAtOrDefault(2);
                    RuntimeValue? buttonsVal = args.ElementAtOrDefault(3);

                    // Check types
                    if (bodyVal != null && bodyVal?.Type != Values.ValueType.String)
                        throw new RuntimeException(new()
                        {
                            Location = Handlers.Helpers.GetLocation(bodyVal?.Location, expr?.Location),
                            Error = $"Argument 1 must be of type string"
                        });
                    if (titleVal != null && titleVal?.Type != Values.ValueType.String)
                        throw new RuntimeException(new()
                        {
                            Location = Handlers.Helpers.GetLocation(titleVal?.Location, expr?.Location),
                            Error = $"Argument 2 must be of type string"
                        });
                    if (iconVal != null && iconVal?.Type != Values.ValueType.Int)
                        throw new RuntimeException(new()
                        {
                            Location = Handlers.Helpers.GetLocation(iconVal?.Location, expr?.Location),
                            Error = $"Argument 3 must be of type integer"
                        });

                    if (buttonsVal != null && buttonsVal?.Type != Values.ValueType.Int)
                        throw new RuntimeException(new()
                        {
                            Location = Handlers.Helpers.GetLocation(buttonsVal?.Location, expr?.Location),
                            Error = $"Argument 4 must be of type integer"
                        });

                    string? body = ((StringValue?)bodyVal ?? null)?.Value ?? "";
                    string? title = ((StringValue?)titleVal ?? null)?.Value ?? "";
                    int icon = (int)(((IntegerValue?)iconVal ?? null)?.Value ?? 0);
                    int buttons = (int)(((IntegerValue?)buttonsVal ?? null)?.Value ?? 0);

                    int result = MessageBox((IntPtr)0, body == null ? "" : body, title == null ? "" : title, icon + buttons);
                    return Helpers.CreateInteger(result);
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

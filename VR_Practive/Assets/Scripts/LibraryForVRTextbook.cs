using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.CompilerServices;

namespace UnityVR
{
    public class LibraryForVRTextbook : MonoBehaviour
    {

        public static string GetSourceFileName([CallerFilePath] string sourceFilepath = "")
            => Path.GetFileName(sourceFilepath.Replace(@"\", "/"));

        public static string GetCallerMember([CallerMemberName] string memberName = "")
            => memberName;
    }
}


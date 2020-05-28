using System;
using System.IO;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Services
{
    public enum PatchStatus
    {
        NotSupported,
        Original,
        Patched
    }

    public class PatchInfo
    {
        public PatchStatus Status { get; set; } = PatchStatus.NotSupported;
        public FileInfo File { get; set; }
        public long Index { get; set; } = -1;
    }

    public class LocalizationService
    {
        public static LocalizationService Instance { get; } = new LocalizationService();
        private LocalizationService(){}

        public PatchInfo GetPatchSupport(GameInfo gameInfo)
        {
            using var stream = gameInfo.ExeFile.OpenRead();
            //check for original
            var index = StreamHelper.IndexOf(stream, SettingsService.Instance.AppSettings.OriginalPattern);
            if (index > 0)
                return new PatchInfo { Status = PatchStatus.Original, File = gameInfo.ExeFile, Index = index};
            //check for patch
            stream.Seek(0, SeekOrigin.Begin);
            index = StreamHelper.IndexOf(stream, SettingsService.Instance.AppSettings.PatchPattern);
            if (index > 0)
                return new PatchInfo { Status = PatchStatus.Patched, File = gameInfo.ExeFile, Index = index };
            // not found any
            return new PatchInfo();
        }

        public PatchInfo Patch(PatchInfo patchInfo)
        {
            if (patchInfo.Status == PatchStatus.Original)
            {
                if (StreamHelper.UpdateFile(patchInfo.File, patchInfo.Index, SettingsService.Instance.AppSettings.PatchPattern))
                {
                    patchInfo.Status = PatchStatus.Patched;
                }
            }
            else if (patchInfo.Status == PatchStatus.Patched)
            {
                if (StreamHelper.UpdateFile(patchInfo.File, patchInfo.Index, SettingsService.Instance.AppSettings.OriginalPattern))
                {
                    patchInfo.Status = PatchStatus.Original;
                }
            }

            return patchInfo;
        }
    }
}
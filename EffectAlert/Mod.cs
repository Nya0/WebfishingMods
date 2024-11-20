using GDWeave;
using System.Text.Json.Serialization;

namespace EffectAlert;

public class Config // https://github.com/ChrisFeline/WebfishingMod-MetalGearAlert
{
    [JsonInclude] public float volume = 10;

    public static float LinearToDecibel(float linear)
    {
        float dB;
        if (linear != 0) dB = 20.0f * (float)Math.Log10(linear);
        else dB = -144.0f;
        return dB;
    }

    public int GetVolumeDB()
    {
        return (int)Math.Round(LinearToDecibel(Math.Clamp(volume, 0f, 100f) / 100f));
    }
}

public class Mod : IMod {
    public static Config Config { get; private set; }
    public Mod(IModInterface modInterface) {
        Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new StatusFXPatch(modInterface));
    }

    public void Dispose() { }
}

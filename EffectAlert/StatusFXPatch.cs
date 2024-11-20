using GDWeave;
using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace EffectAlert;

public class StatusFXPatch : IScriptMod {
    Config config = new Config();

    private const string Path = "/root/n0EffectAlert";
    private const string SoundPath = "res://mods/n0.EffectAlert/Resources/Bass_TickTock.mp3";
    private const string NotifSound = "notifsound";

    public bool ShouldRun(string path) => path == "res://Scenes/HUD/StatusEffectbox/statusfxbox.gdc";

    // https://github.com/danielah05/WebfishingMods
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var newLineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);
        var colonConsumer = new TokenConsumer(t => t.Type is TokenType.Colon);


        var setupFuncMatch = new FunctionWaiter("_setup");

        foreach (var token in tokens) {

            if (setupFuncMatch.Check(token)) {
                // found match

                yield return token; // ignore new line i think

                // -- Effect Started --
                // if tier > 0 and not visible: 
                /*
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("tier");
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.OpAnd);

                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("get");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("visible"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                */

                // -- Effect Ended --
                // if tier == 0 and visible:

                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("tier");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.OpAnd);

                yield return new IdentifierToken("get");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("visible"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);

                //  $Path._play_sound(SoundPath)
                yield return new Token(TokenType.Newline, token.AssociatedData + 1);
                yield return new Token(TokenType.Dollar);
                yield return new IdentifierToken(Path);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_play_sound");

                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant(SoundPath));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(Mod.Config.GetVolumeDB()));
                yield return new Token(TokenType.ParenthesisClose);



                modInterface.Logger.Information("Done patching");


                yield return token;
            } else {
                // return to original token
                yield return token;
            }
        }
    }

    public IModInterface modInterface;
    public StatusFXPatch(IModInterface pmodInterface)
    {
        modInterface = pmodInterface;
    }
}

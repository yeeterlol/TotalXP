using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace TotalXP;

public class GrabTotalXP : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

    // returns a list of tokens for the new script, with the input being the original script's tokens
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var waiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken {Name: "_process"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken {Name: "delta"},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline
        ]);

        var bbcode_text = new MultiTokenWaiter([
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: StringVariant { Value: "[center][color=#ffeed5]$ [color=#d5aa73]"}},
            t => t.Type is TokenType.OpAdd,
            t => t is IdentifierToken {Name: "cash"},
        ]);

        // loop through all tokens in the script
        foreach (var token in tokens) {
            if (waiter.Check(token))
            {
                // found our match, return the original newline
                yield return token;

                // var total_xp = 0
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("total_xp");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Newline, 1);

                // for i in range(PlayerData.)
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpIn);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.GenRange);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("badge_level");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);
                yield return new Token(TokenType.CfContinue);
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("total_xp");
                yield return new Token(TokenType.OpAssignAdd);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_get_xp_goal");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 1);

                yield return new IdentifierToken("total_xp");
                yield return new Token(TokenType.OpAssignAdd);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("badge_xp");
                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.Dollar);
                yield return new IdentifierToken("main");
                yield return new Token(TokenType.OpDiv);
                yield return new IdentifierToken("menu");
                yield return new Token(TokenType.OpDiv);
                yield return new IdentifierToken("Panel4");
                yield return new Token(TokenType.OpDiv);
                yield return new IdentifierToken("cashlabel");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("fit_content_height");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                // don't forget another newline!
                yield return new Token(TokenType.Newline, 1);

            }
            else if (bbcode_text.Check(token)) {
                yield return token;

                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("\n total xp: "));
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(" "));
                yield return new Token(TokenType.OpAdd);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("total_xp");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(" "));
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("xp"));
                yield return new Token(TokenType.Newline, 1);

            }
            else {
                // return the original token
                yield return token;
            }
        }
    }
}

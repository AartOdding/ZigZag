using ZigZag.Core;


/*
Lorem ipsum dolor sit amet, consectetuer adipiscing elit.
Aenean commodo ligula eget dolor.
Aenean massa.
Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.
Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.
Nulla consequat massa quis enim.
Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu.
In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo.
Nullam dictum felis eu pede mollis pretium. Integer tincidunt.
Cras dapibus. Vivamus elementum semper nisi.
Aenean vulputate eleifend tellus.
Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim.
Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus.
Phasellus viverra nulla ut metus varius laoreet.
Quisque rutrum.
Aenean imperdiet.
Etiam ultricies nisi vel augue.
Curabitur ullamcorper ultricies nisi.
Nam eget dui.
*/

namespace ZigZag.Text
{
    public class LoremIpsum : Node
    {
        public TextData Output;

        public LoremIpsum()
        {
            Output = new TextData();
            Output.Lines.Add("Lorem ipsum dolor sit amet, consectetuer adipiscing elit.");
            Output.Lines.Add("Aenean commodo ligula eget dolor.");
            Output.Lines.Add("Aenean massa.");
            Output.Lines.Add("Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.");
            Output.Lines.Add("Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.");
            Output.Lines.Add("Nulla consequat massa quis enim.");
            Output.Lines.Add("Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu.");
            Output.Lines.Add("In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo.");
            Output.Lines.Add("Nullam dictum felis eu pede mollis pretium.Integer tincidunt.");
            Output.Lines.Add("Cras dapibus. Vivamus elementum semper nisi.");
            Output.Lines.Add("Aenean vulputate eleifend tellus.");
            Output.Lines.Add("Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim.");
            Output.Lines.Add("Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus.");
            Output.Lines.Add("Phasellus viverra nulla ut metus varius laoreet.");
            Output.Lines.Add("Quisque rutrum.");
            Output.Lines.Add("Aenean imperdiet.");
            Output.Lines.Add("Etiam ultricies nisi vel augue.");
            Output.Lines.Add("Curabitur ullamcorper ultricies nisi.");
            Output.Lines.Add("Nam eget dui.");
        }

        public override void Update()
        {

        }
    }
}

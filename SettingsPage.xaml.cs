using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace AEAQuiz
{
    public class Settings { }
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            string schemaJson = @"{
  'description': 'A person',
  'type': 'object',
  'properties':
  {
    'name': {'type':'string'},
    'hobbies': {
      'type': 'array',
      'items': {'type':'string'}
    }
  }
}";

            JsonSchema schema = JsonSchema.Parse(schemaJson);

            JObject person = JObject.Parse(@"{
  'name': 'James',
  'hobbies': ['.NET', 'Blogging', 'Reading', 'Xbox', 'LOLCATS']
}");

            bool valid = person.IsValid(schema);
            if (!valid)
            {
                JsonResult.Text = "JSON not so good";
            }
            else
            {
                JsonResult.Text = $"{person["name"]} has Hobbies: \n\t";
                foreach (var item in person["hobbies"]) 
                {
                    JsonResult.Text += $"{item}, ";
                }
            }
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
            rotatingLabel.Rotation = value;
            displayLabel.Text = String.Format("The Slider value is {0}", value);
        }
    }
}


const int kCh1DefaultButton = 13;
const int kCh1CommsButton = 12;
const int kCh1DefefaultLed = 5;
const int kCh1CommsLed = 4;
const int kCh1Pot = A0;

const int kCh2DefaultButton = 11;
const int kCh2CommsButton = 10;
const int kCh2DefefaultLed = 3;
const int kCh2CommsLed = 2;
const int kCh2Pot = A1;

const int kCh3DefaultButton = 9;
const int kCh3CommsButton = 8;
const int kCh3DefefaultLed = 1;
const int kCh3CommsLed = 0;
const int kCh3Pot = A2;

const int deBounceDelay = 50;
const int analogChangeThreshold = 5;

const int numberOfButtons = 6;
// input pin, previous value of input button, last changed,  corresponding outpur pin,
int buttonMapping[numberOfButtons][4] = 
{
  {kCh1DefaultButton, 0, 0, kCh1DefefaultLed},
  {kCh1CommsButton, 0,0, kCh1CommsLed},
  {kCh2DefaultButton ,0 ,0 ,kCh2DefefaultLed},
  {kCh2CommsButton, 0 ,0 ,kCh2CommsLed},
  {kCh3DefaultButton ,0, 0, kCh3DefefaultLed},
  {kCh3CommsButton, 0, 0, kCh3CommsLed},
};

const int numberOfPots = 3;
// input pin, previous value of pot, last change
int potMapping[numberOfPots][3] = 
{
  {kCh1Pot, 0, 0},
  {kCh2Pot, 0, 0},
  {kCh2Pot, 0, 0},
};

void setup() {
  pinMode(kCh1DefaultButton, INPUT);
  digitalWrite(kCh1DefaultButton, HIGH);
  pinMode(kCh1CommsButton, INPUT);
  digitalWrite(kCh1CommsButton, HIGH);
  pinMode(kCh1DefefaultLed, OUTPUT);
  pinMode(kCh1CommsLed, OUTPUT);
  pinMode(kCh1Pot, INPUT);

  pinMode(kCh2DefaultButton, INPUT);
  digitalWrite(kCh2DefaultButton, HIGH);
  pinMode(kCh2CommsButton, INPUT);
  digitalWrite(kCh2CommsButton, HIGH);
  pinMode(kCh2DefefaultLed, OUTPUT);
  pinMode(kCh2CommsLed, OUTPUT);
  pinMode(kCh2Pot, INPUT);

  pinMode(kCh3DefaultButton, INPUT);
  digitalWrite(kCh3DefaultButton, HIGH);
  pinMode(kCh3CommsButton, INPUT);
  digitalWrite(kCh3CommsButton, HIGH);
  pinMode(kCh3DefefaultLed, OUTPUT);
  pinMode(kCh3CommsLed, OUTPUT);
  pinMode(kCh3Pot, INPUT);

  Serial.begin(9600);

}

void loop() {


  for(int i = 0; i < numberOfButtons ; i++)
  {
    int currentValue = digitalRead(buttonMapping[i][0]);
    bool buttonOn = (currentValue == LOW);
    // if(buttonOn)
    // {
    //   digitalWrite(buttonMapping[i][1], HIGH);
    // }
    // else
    // {
    //   digitalWrite(buttonMapping[i][1], LOW);
    // }

    int previousValue = buttonMapping[i][1];
    if(currentValue != previousValue)
    {
      int lastChanged = buttonMapping[i][2];
      int now = millis();
      if(now - lastChanged > deBounceDelay)
      {
        String buttonIdentifier = "button";
        buttonIdentifier = buttonIdentifier + i;
        SendMessage(buttonIdentifier, currentValue);
        buttonMapping[i][1] = currentValue;
        buttonMapping[i][2]= now;

      }
    }
  }

  for(int i = 0; i < numberOfPots ; i++)
  {
    int currentValue = map(analogRead(potMapping[i][0]),0,1023,0,255);

    int previousValue = potMapping[i][1];
    if(currentValue != previousValue && abs(currentValue - previousValue) > analogChangeThreshold)
    {
      int lastChanged = potMapping[i][2];
      int now = millis();
      if(now - lastChanged > deBounceDelay)
      {
        String potIdentifier = "pot";
        potIdentifier = potIdentifier + i;
        SendMessage(potIdentifier, currentValue);
        potMapping[i][1] = currentValue;
        potMapping[i][2]= now;
      }
    }
  }
}

void SendMessage(String inputIdentifier, int value)
{
  String message = "{\"Input\":\"";
  message = message + inputIdentifier;     
  message = message + "\",\"Value\":";
  message = message + value;
  message += "}\r\n";
  Serial.print(message);
}

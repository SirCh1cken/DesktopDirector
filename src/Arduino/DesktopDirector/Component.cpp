#include "Arduino.h"
#include "Component.h"
#include "ComponentType.cpp"

Component::Component (String componentType, int arduinoPin, int multiplexerPin, String componentName)
{
  _componentType = componentType;
  _arduinoPin = arduinoPin;
  _multiplexerPin = multiplexerPin;
  _componentName = componentName;
}

void Component::Setup()
{
  if(_componentType == ComponentTypeButton)
  {
    pinMode(_arduinoPin, INPUT);
    digitalWrite(_arduinoPin, HIGH);
  }
  else if(_componentType == ComponentTypeLed)
  {
    pinMode(_arduinoPin, OUTPUT);
  }
  else if(_componentType == ComponentTypePotentiometer)
  {
    pinMode(_arduinoPin, INPUT);
  }
}

String Component::ReportConfiguration()
{
  String configurationString ="{\"Name\":\"";
  configurationString += _componentName;
  configurationString += "\",\"ComponentType\":\"";
  configurationString += _componentType;
  configurationString += "\",\"Address\":\"";
  configurationString += _arduinoPin;
  configurationString += "x";
  configurationString += _multiplexerPin;
  configurationString += "\"}";
  return configurationString;
  //return "{\"Name\":\"button0\",\"ComponentType\":\"button\",\"Address\":\"d1\"},";
}

void Component::Read()
{
  if(_componentType == ComponentTypeButton)
  {
    ReadButton();
  }
  else if(_componentType == ComponentTypePotentiometer)
  {
    ReadPotentiometer();
  }
}

void Component::ReadButton()
{
  int currentValue = digitalRead(_arduinoPin);
  bool buttonOn = (currentValue == LOW);

  if(currentValue != _previousValue)
  {
    int now = millis();
    if(now - _lastChanged > kDebounceDelay)
    {
      bool buttonOn = (currentValue == 0);

      _previousValue = currentValue;
      _lastChanged = now;

      String message = "{\"Input\":\"";
      message += _componentName;     
      message += "\",\"Value\":";
      message += buttonOn;
      message += "}\r\n";
      Serial.print(message);
    }
  }
}

void Component::ReadPotentiometer()
{
  int currentValue = map(analogRead(_arduinoPin),0,1023,0,255);

  if(currentValue != _previousValue && abs(currentValue - _previousValue) > kAnalogChangeThreshold)
  {
    int now = millis();
    if(now - _lastChanged > kDebounceDelay)
    {
      _previousValue = currentValue;
      _lastChanged = now;

      String message = "{\"Input\":\"";
      message += _componentName;
      message += "\",\"Value\":";
      message += currentValue;
      message += "}\r\n";
      Serial.print(message);
    }
  }
}

void Component::ProcessMessage(String message)
{
  Serial.println(message);
  if(_componentType == ComponentTypeLed)
  {
    if(message=="on")
    {
      digitalWrite(_arduinoPin, HIGH);
    }
    else if(message="off")
    {
      digitalWrite(_arduinoPin, LOW);
    }
  }
}

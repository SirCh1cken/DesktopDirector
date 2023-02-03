#include "Component.h"
#include "ComponentType.cpp"

const int numberOfComponents = 10;

Component components[] = {
  // Component(ComponentTypeButton, 0, -1,"Button0"),
  // Component(ComponentTypeButton, 1, -1,"Button1"),
  Component(ComponentTypeButton, 2, -1,"Button2"),
  Component(ComponentTypeButton, 3, -1,"Button3"),
  Component(ComponentTypeButton, 4, -1,"Button4"),
  Component(ComponentTypeButton, 5, -1,"Button5"),
  Component(ComponentTypeButton, 6, -1,"Button6"),
  Component(ComponentTypeButton, 7, -1,"Button7"),
  Component(ComponentTypeLed, 8, -1,"LedWhite"),
  Component(ComponentTypeLed, 9, -1,"LedRed"),
  Component(ComponentTypeLed, 10, -1,"LedYellow"),
  Component(ComponentTypeLed, 11, -1,"LedBlue")
};


void setup() {
  for(int i=0; i < numberOfComponents; i ++)
  {
    components[i].Setup();
  }

  Serial.begin(9600);
}

void loop() {
  ProcessRecievedMessages();
  ProcessSensors();
}

void ProcessRecievedMessages()
{
  if(Serial.available() > 0)  
  {          
    String receivedMessage = Serial.readStringUntil('\n');
    
    if(receivedMessage.startsWith("component-message"))
    {
      ProcessComponentMessage(receivedMessage);
    }
    else if(receivedMessage.startsWith("request-configuration"))
    {
      SendConfiurationMessage();
    }

  }
}

void ProcessSensors()
{
  for(int i=0; i < numberOfComponents; i ++)
  {
    components[i].Read();
  }
}

void SendConfiurationMessage()
{
  Serial.print("[");

  for(int i=0; i < numberOfComponents; i ++)
  {
    String componentConfiguration = components[i].ReportConfiguration();
    Serial.print(componentConfiguration);
    if(i+1 < numberOfComponents)
    {
      Serial.print(",");
    }
  }

  Serial.print("]\r\n");
}

void ProcessComponentMessage(String message)
{
  Serial.println(message);

  int firstSeparatorIndex = message.indexOf(",");
  String messageWithoutPrefix = message.substring(firstSeparatorIndex+1);
  int secondSeparatorIndex = messageWithoutPrefix.indexOf(",");
  String componentName = messageWithoutPrefix.substring(0,secondSeparatorIndex);
  String messageForComponent = messageWithoutPrefix.substring(secondSeparatorIndex+1);

  for(int i=0; i < numberOfComponents; i ++)
  {
    if(components[i]._componentName == componentName)
    {
      components[i].ProcessMessage(messageForComponent);
    }
  }
}

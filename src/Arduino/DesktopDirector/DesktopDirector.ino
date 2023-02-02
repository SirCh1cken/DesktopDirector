#include "Component.h"
#include "ComponentType.cpp"

const int numberOfComponents = 15;

Component components[] = {
  Component(ComponentTypeLed, 0, -1,"Led1"),
  Component(ComponentTypeLed, 1, -1,"Led2"),
  Component(ComponentTypeLed, 2, -1,"Led3"),
  Component(ComponentTypeLed, 3, -1,"Led4"),
  Component(ComponentTypeLed, 4, -1,"Led5"),
  Component(ComponentTypeLed, 5, -1,"Led6"),
  Component(ComponentTypeButton, 8, -1,"Button1"),
  Component(ComponentTypeButton, 9, -1,"Button2"),
  Component(ComponentTypeButton, 10, -1,"Button3"),
  Component(ComponentTypeButton, 11, -1,"Button4"),
  Component(ComponentTypeButton, 12, -1,"Button5"),
  Component(ComponentTypeButton, 13, -1,"Button6"),
  Component(ComponentTypePotentiometer, A0, -1,"Potentiometer1"),
  Component(ComponentTypePotentiometer, A1, -1,"Potentiometer2"),
  Component(ComponentTypePotentiometer, A2, -1,"Potentiometer3")
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

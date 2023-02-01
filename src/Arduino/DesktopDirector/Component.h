#ifndef Component_h
  #define Component_h
  #include "Arduino.h" 
class Component {
  public:
    Component(String componentType, int arduinoPin, int multiplexerPin, String componentName);
    void Setup();
    String ReportConfiguration ();
    void Read();

  private:
    const int kDebounceDelay = 50;
    const int kAnalogChangeThreshold = 50;
    
    String _componentType;
    int _arduinoPin;
    int _multiplexerPin;
    int _previousValue;
    int _lastChanged;
    String _componentName;
    void ReadButton();
    void ReadPotentiometer();
};
#endif
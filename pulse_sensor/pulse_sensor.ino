int pulsePin = 0;                 // Pulse Sensor purple wire connected to analog pin 0

// these variables are volatile because they are used during the interrupt service routine

volatile int BPM = 60;                   // used to hold the pulse rate
volatile int Signal;                // holds the incoming raw data
volatile int IBI = 600;             // holds the time between beats, the Inter-Beat Interval
volatile boolean Pulse = false;     // true when pulse wave is high, false when it's low
volatile boolean new_bpm = false;        // becomes true when Arduoino finds a beat.
String outputString;
String spacer = ", ";

void setup() {
    Serial.begin(115200);
    interruptSetup();                 // set up to read Pulse Sensor signal every 2mS 
    // UN-COMMENT THE NEXT LINE IF YOU ARE POWERING The Pulse Sensor AT LOW VOLTAGE, 
    // AND APPLY THAT VOLTAGE TO THE A-REF PIN
    //analogReference(EXTERNAL);   
}

void loop(){
    outputString = String(BPM) + String(", ") + String(Signal) + String(", ") + String(IBI);
    Serial.println(outputString);
    Serial.flush();
    if (new_bpm == true){                       // Quantified Self flag is true when arduino finds a heartbeat
        new_bpm = false;                      // reset the Quantified Self flag for next time    
    }  
}
package scxmlgen.Modalities;

import scxmlgen.interfaces.IOutput;



public enum Output implements IOutput{
    
    //SQUARE_RED("[shape][SQUARE][color][RED]"),
    //SQUARE_BLUE("[shape][SQUARE][color][BLUE]"),
    //SQUARE_YELLOW("[shape][SQUARE][color][YELLOW]"),
    //TRIANGLE_RED("[shape][TRIANGLE][color][RED]"),
    //TRIANGLE_BLUE("[shape][TRIANGLE][color][BLUE]"),
    //TRIANGLE_YELLOW("[shape][TRIANGLE][color][YELLOW]"),
    //CIRCLE_RED("[shape][CIRCLE][color][RED]"),
    //CIRCLE_BLUE("[shape][CIRCLE][color][BLUE]"),
    //CIRCLE_YELLOW("[shape][CIRCLE][color][YELLOW]"),
    //CIRCLE("[shape][CIRCLE]")
    
    TOCAR_ANZOL("[tocar][tocarAnzol][reproduzir]"),
    TOCAR_ENGATE("[tocar][tocarEngate][reproduzir]"),
    TOCAR_CIRCO("[tocar][tocarCirco][reproduzir]");
    
    
    
    private String event;

    Output(String m) {
        event=m;
    }
    
    @Override
    public String getEvent(){
        return this.toString();
    }

    @Override
    public String getEventName(){
        return event;
    }
}

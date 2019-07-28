/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package genfusionscxml;

import java.io.IOException;
import scxmlgen.Fusion.FusionGenerator;
import scxmlgen.Modalities.Output;
import scxmlgen.Modalities.Speech;
import scxmlgen.Modalities.SecondMod;

/**
 *
 * @author nunof
 */
public class GenFusionSCXML {

    /**
     * @param args the command line arguments
     * @throws java.io.IOException
     */
    public static void main(String[] args) throws IOException {

    FusionGenerator fg = new FusionGenerator();
  
    
    //fg.Sequence(Speech.SQUARE, SecondMod.RED, Output.SQUARE_RED);
    //fg.Sequence(Speech.SQUARE, SecondMod.BLUE, Output.SQUARE_BLUE);
    //fg.Sequence(Speech.SQUARE, SecondMod.YELLOW, Output.SQUARE_YELLOW);
    //fg.Complementary(Speech.TRIANGLE, SecondMod.RED, Output.TRIANGLE_RED);
    //fg.Complementary(Speech.TRIANGLE, SecondMod.BLUE, Output.TRIANGLE_BLUE);
    //fg.Complementary(Speech.TRIANGLE, SecondMod.YELLOW, Output.TRIANGLE_YELLOW);
    //fg.Complementary(Speech.CIRCLE, SecondMod.RED, Output.CIRCLE_RED);
    //fg.Complementary(Speech.CIRCLE, SecondMod.BLUE, Output.CIRCLE_BLUE);
    //fg.Complementary(Speech.CIRCLE, SecondMod.YELLOW, Output.CIRCLE_YELLOW);
    
    //fg.Single(Speech.CIRCLE, Output.CIRCLE);  //EXAMPLE
    //fg.Redundancy(Speech.CIRCLE, SecondMod.CIRCLE, Output.CIRCLE);  //EXAMPLE
    
    fg.Sequence(Speech.ANZOL, SecondMod.REPRODUZIR, Output.TOCAR_ANZOL);
    fg.Sequence(Speech.ENGATE, SecondMod.REPRODUZIR, Output.TOCAR_ENGATE);
    fg.Sequence(Speech.CIRCO, SecondMod.REPRODUZIR, Output.TOCAR_CIRCO);
    fg.Complementary(Speech.ANZOL, SecondMod.REPRODUZIR, Output.TOCAR_ANZOL);
    fg.Complementary(Speech.ENGATE, SecondMod.REPRODUZIR, Output.TOCAR_ENGATE);
    fg.Complementary(Speech.CIRCO, SecondMod.REPRODUZIR, Output.TOCAR_CIRCO);
    
    fg.Build("fusion.scxml");
        
        
    }
    
}

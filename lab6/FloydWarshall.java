package lab6;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;

public class FloydWarshall {
    int n;        
    int[][][] pathcArray;        
    FloydWarshall(String path) throws IOException {
        intil(path); 
    }        
        public int[][] getFirstPatch() {            
            return  pathcArray[0]; 
        }        
            public int getSize() {            
                return n; 
            }        
    private void intil(String string) throws IOException {   
        Path path = Paths.get(string);           
        List<String> contents = Files.readAllLines(path);            
        n = contents.size() - 1;        
        pathcArray = new int[n+1][n][n];           
        for(int i = 0; i < n ; i++) {               
            String[] row = contents.get(i+1).split(" ");                
            for(int j = 0; j < n; j++) {                    
                pathcArray[0][i][j] = Integer.MAX_VALUE;                   
                if(i == j)                        
                pathcArray[0][i][j] = 0;                    
                else                        
                pathcArray[0][i][j] = Integer.parseInt(row[j+1])!=0? Integer.parseInt(row[j+1]):pathcArray[0][i][j]; 
               }
            }
        }

    public int FloydMin(int i1, int j1) {           
        for (int k = 1; k <= n; k++)               
            for (int i = 0; i < n; i++)                    
                for (int j = 0; j < n; j++)                        
                if (pathcArray[k-1][i][k-1]==Integer.MAX_VALUE||pathcArray[k-1][k-1][j]==Integer.MAX_VALUE)                            
                pathcArray[k][i][j]= pathcArray[k-1][i][j];                        
                else                         
            pathcArray[k][i][j]= Math.min(pathcArray[k-1][i][j],pathcArray[k-1][i][k-1]+pathcArray[k-1][k-1][j]);            
        return pathcArray[n][i1][j1];
    }
}
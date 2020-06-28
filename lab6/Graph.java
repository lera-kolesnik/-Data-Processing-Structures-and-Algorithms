package lab6;
import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;

public class Graph extends JFrame {
    ArrayList<Point> point;    
    int size;    
    int n;    
    int[][] firtsWay;    
    Graph(int size, FloydWarshall alg) {        
        n = alg.getSize();        
        firtsWay = new int[n][n];        
        firtsWay = alg.getFirstPatch();        
        this.size = size;       
        setTitle("Floyd-Warshall algorithm");        
        setSize(size,size);        
        setVisible(true);        
        setDefaultCloseOperation(EXIT_ON_CLOSE);    
    }    
    @Override    
    public void paint(Graphics g) {        
        int R = size / 2 - size / 5;        
        int X = size / 2, Y = size / 2;        
        point = new ArrayList<Point>();        
        g.setFont(new Font("TimesRoman", Font.PLAIN, 18));        
        double angle = 360.0 / n;        
        for (int i = 0; i < n; i++) {            
            int x = (int) (X + R * Math.cos(Math.toRadians(angle * i)));           
            int y = (int) (Y + R * Math.sin(Math.toRadians(angle * i)));            
            point.add(new Point(x, y));        
        }        
        for (int i = 0; i < n; i++) {            
            for(int j = i; j < n; j++) {                
                if(i != j && firtsWay[i][j] != Integer.MAX_VALUE) {                    
                    int x1 = point.get(i).x;                    
                    int y1 = point.get(i).y;                    
                    int x2 = point.get(j).x;                    
                    int y2 = point.get(j).y;                    
                    g.setColor(Color.BLACK);                    
                    g.drawLine(x1, y1, x2, y2);                
                }            
            }        
        }        
        int l = 0;        
        for (Point p : point) {            
            g.setColor(Color.WHITE);            
            g.fillOval(p.x - 25, p.y - 25, 50, 50);            
            g.setColor(Color.BLACK);            
            g.drawOval(p.x - 25, p.y - 25, 50, 50);            
            g.setColor(Color.BLACK);            
            g.drawString(String.valueOf(l + 1), p.x - 5, p.y + 5);            
            l++;        
        }    
    }
}
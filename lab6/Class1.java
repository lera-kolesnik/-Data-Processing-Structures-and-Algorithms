package lab6;
import java.util.Scanner;
import java.io.IOException;

public class Class1 {  
    public static void main (String[] args) throws IOException {
        Scanner scanner = new Scanner(System.in);
        System.out.println("Введите начальную точку");        
        int firstDot = scanner.nextInt();       
        System.out.println("Введите конечную точку");       
        int lastDot = scanner.nextInt();        
        long startTime = System.nanoTime();        
        FloydWarshall alg = new FloydWarshall ("/Users/kolesnikvaleria/Desktop/Input.txt");     
        long endTime = System.nanoTime()-startTime;        
        int size = alg.getSize();        
        Graph floyd = new Graph(800,alg);        
        if (firstDot<=size&&lastDot<=size)         
         System.out.println("Минимальное расстоняние = " + alg.FloydMin(firstDot, lastDot)+"\nВремя выполнения = "+endTime+" нс");
         else        
          System.out.println("Точки не существует");    
    }
}

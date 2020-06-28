package lab4;
import java.io.FileReader;
import java.io.IOException;

//Стек/очередь, с принципом работы первый пришел - последним уйдешь :(
// => новый элемент записывается в начало

class Stack{
    private int stackSize;
    private char[] stackArray;
    private int top;

    public Stack(int n){
        this.stackSize=n;
        this.stackArray = new char[stackSize];
        this.top = -1;
    }

    public void addElement(char element){
        stackArray[++top] = element;
    }//Добавляем элемент в стек

    public char deleteElement(){
        return stackArray[top--];
    }//удаляем элемент из стека

    public char getTop(){
        return stackArray[top];
    }//Получаем первый элемент стека

    public boolean isEmpty(){
        return (top == - 1);
    }//Проверяем стек на пустоту

    //Проверяем стек на заполненность
    public boolean isFull(){ return (top == stackSize-1); }

}

public class lab4siaod {
    public static Stack fillStack() {
        String eStr = "";
        try(FileReader reader = new FileReader("D:\\Input.txt"))
        {
            int c;
            while((c=reader.read())!=-1){
                char cChar=(char)c;
                eStr += (cChar);
            }
        }
        catch(IOException ex){
            System.out.println(ex.getMessage());
        }
        Stack fileText = new Stack(eStr.length());
        for (int j = eStr.length() - 1; j >-1; j--)
            fileText.addElement(eStr.charAt(j));
        return fileText;
    }
    public static boolean check(Stack fileText) {

        boolean isCorrect=true;
        int countOfBranch = 0;
            while (!fileText.isEmpty()&&isCorrect)
            {
                char c = fileText.getTop();
                if(c=='(')
                    countOfBranch++;
                else if (c==')')
                    countOfBranch--;
                fileText.deleteElement();
                if (countOfBranch<0)
                    isCorrect=false;
            }
        return countOfBranch == 0;
    }

    public static void main(String[] args) {
        Stack resStack = fillStack();
        if (check(resStack))
            System.out.println("Круглые скобки сбалансированны");
        else
            System.out.println("Круглые скобки не сбалансированны");
    }
}

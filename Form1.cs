using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MS_lr
{
    public partial class Form1 : Form
    {
        private List<Student> studentsList;
        private List<Student> asspirantsStudentsList;
        private List<Student> teacherStudentsList;
        private int M;
        private int speed;
        private int t0;
        private int addToAsspirantTime;
        private int time;

        private int n1;
        private int ask_asspirant;
        private int ask_true_asspirant;
        private int t_asspirant;
        private int t1;

        private int n2;
        private int ask_teacher;
        private int ask_true_teacher;
        private int t_teacher;
        private int t2;

        private int redirect_to_students;

        public Form1()
        {
            InitializeComponent();
        }

        private void addToAsspirantList() 
        {
            if (addToAsspirantTime / t0 > 0)
            {
                if (studentsList.Count != 0)
                {
                    addToAsspirantTime = addToAsspirantTime - t0;
                    asspirantsStudentsList.Add(new Student());
                    studentsList.RemoveAt(0);
                    studentsCountLabel.Text = studentsList.Count.ToString();
                    studentsProgressBar.Value = studentsList.Count;
                    QueueAsspirantProgressBar.Value = asspirantsStudentsList.Count;
                }
            }
            else 
            {
                addToAsspirantTime++;
                QueueAsspirantTimeProgressBar.Value = addToAsspirantTime;
            }
        }
        private void askingAsspirant() 
        {
            if (ask_asspirant <= n1)
            {
                AsspirantAskingProgressBar.Value = ask_asspirant;
                if (t_asspirant / t1 > 0)
                {
                    ask_asspirant++;
                    t_asspirant = t_asspirant - t1;
                    Random rnd = new Random();
                    int value = rnd.Next(0, 1);
                    if (value == 1) ask_true_asspirant++;
                }
                else
                {
                    t_asspirant++;
                    AsspirantAskingTimeProgressBar.Value = t_asspirant;
                }
            }
            else 
            {
                if (!((float)ask_true_asspirant / n1 >= 0.3 && (float)ask_true_asspirant / n1 <= 0.5))
                {
                    teacherStudentsList.Add(new Student());
                    teacherCountLabel.Text = teacherStudentsList.Count.ToString();
                }

                asspirantsStudentsList.RemoveAt(0);
                asspirantCountLabel.Text = asspirantsStudentsList.Count.ToString();
                ask_asspirant = 0;
                ask_true_asspirant = 0;
            }
        }
        private void askingTeacher()
        {
            if (ask_teacher <= n2)
            {
                if (t_teacher / t2 < 0)
                    t_teacher++;
                else
                {
                    ask_teacher++;
                    t_teacher = t_teacher - t2;
                    Random rnd = new Random();
                    int value = rnd.Next(0, 1);
                    if (value == 1) ask_true_teacher++;
                }
            }
            else
            {
                if ((float)ask_true_teacher / n2 <= 0.2) {
                    if (redirect_to_students / 2 * t0 < 0)
                        redirect_to_students++;
                    else
                    {
                        redirect_to_students = redirect_to_students - 2 * t0;
                        studentsList.Add(new Student());
                        studentsCountLabel.Text = studentsList.Count.ToString();
                    }
                }
                else
                {
                    teacherStudentsList.RemoveAt(0);
                    teacherCountLabel.Text = teacherStudentsList.Count.ToString();
                    ask_teacher = 0;
                    ask_true_teacher = 0;
                }


            }
        }
        private void RunButton_Click(object sender, EventArgs e)
        {
            asspirantsStudentsList = new List<Student>();
            teacherStudentsList = new List<Student>();

            M = 15;
            studentsList = new List<Student>(M);
            for (int i = 0; i < M; i++)
                studentsList.Add(new Student());

            studentsProgressBar.Minimum = 0;
            studentsProgressBar.Maximum = M;
            studentsProgressBar.Value = studentsList.Count;
            studentsProgressBar.Refresh();

            
            speedSystemScrollBar.Minimum = 100;
            speedSystemScrollBar.Maximum = 3000;
            speedSystemScrollBar.Value = 1000;

            t0 = 600;
            addToAsspirantTime = 0;

            n1 = 5;
            t1 = 300;
            ask_asspirant = 0;
            t_asspirant = 0;
            ask_true_asspirant = 0;

            n2 = 5;
            t2 = 420;
            ask_teacher = 0;
            t_teacher = 0;
            ask_true_teacher = 0;
            redirect_to_students = 0;


            QueueAsspirantTimeProgressBar.Minimum = 0;
            QueueAsspirantTimeProgressBar.Maximum = t0;

            QueueAsspirantProgressBar.Minimum = 0;
            QueueAsspirantProgressBar.Maximum = M;

            AsspirantAskingProgressBar.Minimum = 0;
            AsspirantAskingProgressBar.Maximum = n1;
            AsspirantAskingTimeProgressBar.Minimum = 0;
            AsspirantAskingTimeProgressBar.Maximum = t1;

            speed = speedSystemScrollBar.Value;

            while (studentsList.Count != 0 || asspirantsStudentsList.Count != 0 || teacherStudentsList.Count != 0) 
            {

                addToAsspirantList();

                if (asspirantsStudentsList.Count > 0)
                    askingAsspirant();

                if (teacherStudentsList.Count > 0)
                    askingTeacher();

                time++;
                studentsProgressBar.Value = studentsList.Count;
                if (speed != speedSystemScrollBar.Value)
                    speed = speedSystemScrollBar.Value;
                Thread.Sleep(1);
                        
            }

        }
    }
}


USE registro_notas;

"""
CREATE TABLE roles (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50)
);


CREATE TABLE estudents (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  last_name VARCHAR(50),
  dni VARCHAR(50),
  email VARCHAR(50)
);

CREATE TABLE teachers (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  last_name VARCHAR(50),
  specialty VARCHAR(50),
  email VARCHAR(50)
);

CREATE TABLE subjects (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  description VARCHAR(100),
  id_teacher INT,
  FOREIGN KEY (id_teacher) REFERENCES Teachers(id)
);


CREATE TABLE courses (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  id_teacher INT,
  FOREIGN KEY (id_teacher) REFERENCES Teachers(id)
);

CREATE TABLE notes (
  id INT IDENTITY(1,1) PRIMARY KEY,
  note_1 DECIMAL(5, 2),
  note_2 DECIMAL(5, 2),
  note_3 DECIMAL(5, 2),
  note_4 DECIMAL(5, 2),
  id_student INT,
  id_course INT,
  FOREIGN KEY (id_student) REFERENCES estudents(id),
  FOREIGN KEY (id_course) REFERENCES courses(id)
);

CREATE TABLE users (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  password VARCHAR(50),
  id_role INT,
  id_student INT,
  id_teacher INT,
  FOREIGN KEY (id_role) REFERENCES roles(id),
  FOREIGN KEY (id_student) REFERENCES estudents(id),
  FOREIGN KEY (id_teacher) REFERENCES teachers(id)
);
"""

-- Crear tabla students_has_subjects
CREATE TABLE students_has_subjects (
  id_student INT,
  id_subject INT,
  FOREIGN KEY (id_student) REFERENCES Estudents(id),
  FOREIGN KEY (id_subject) REFERENCES Subjects(id),
  PRIMARY KEY (id_student, id_subject)
);

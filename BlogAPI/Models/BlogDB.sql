use [BlogDB]

ALTER DATABASE [test] MODIFY NAME = [BlogDB]

-- Create Categories Table
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Create Authors Table
CREATE TABLE Authors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL
);

-- Create Posts Table
CREATE TABLE Posts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CategoryId INT NOT NULL,
    AuthorId INT NOT NULL,
    CONSTRAINT FK_Posts_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    CONSTRAINT FK_Posts_Authors FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
);


-- Insert data into Categories
INSERT INTO Categories (Name) VALUES 
('Technology'),
('Science'),
('Health'),
('Education');

-- Insert data into Authors
INSERT INTO Authors (FullName) VALUES 
('John Doe'),
('Jane Smith'),
('Alice Johnson'),
('Bob Brown');

-- Insert data into Posts
INSERT INTO Posts (Title, Content, CategoryId, AuthorId) VALUES 
('The Future of AI', 'Content about AI', 1, 1), 
('Quantum Physics Explained', 'Content about Quantum Physics', 2, 2),
('Health Benefits of Yoga', 'Content about Yoga', 3, 3),
('Innovations in Online Learning', 'Content about E-Learning', 4, 4),
('AI and Ethics', 'Content about Ethics in AI', 1, 1),
('Advancements in Space Exploration', 'Content about Space', 2, 2),
('Mental Health Awareness', 'Content about Mental Health', 3, 3),
('Challenges in Education Technology', 'Content about EdTech', 4, 4);



--drop table Posts
--drop table Authors
--drop table Categories

select * from Posts
Select * from Authors
Select * from Categories

-- SQL Query to retrieve all posts by a specific author
SELECT 
    p.Id, 
    p.Title, 
    p.Content, 
    c.Name AS CategoryName, 
    a.FullName AS AuthorName
FROM 
    Posts p
JOIN 
    Categories c ON p.CategoryId = c.Id
JOIN 
    Authors a ON p.AuthorId = a.Id
WHERE 
    p.AuthorId = 1;

-- SQL Query to update the category of a specific post
UPDATE 
    Posts
SET 
    CategoryId = 1
WHERE 
    Id = 1;


-- SQL Query to calculate the total number of posts in each category
SELECT 
    c.Name AS CategoryName, 
    COUNT(p.Id) AS TotalPosts
FROM 
    Categories c
LEFT JOIN 
    Posts p ON c.Id = p.CategoryId
GROUP BY 
    c.Name;


	BACKUP DATABASE [YourDatabaseName]
TO DISK = 'D:\Backups\YourDatabaseBackup.bak'
WITH FORMAT,
     NAME = 'Full Backup of YourDatabaseName',
     SKIP,
     NOREWIND,
     NOUNLOAD,
     STATS = 10;
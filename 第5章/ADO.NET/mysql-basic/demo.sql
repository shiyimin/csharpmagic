USE DemoMysqlDb;

DROP TABLE IF EXISTS `tblCSharp`;

CREATE TABLE `tblCSharp` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(200) NOT NULL,
  `chars` int(11),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=109 DEFAULT CHARSET=utf8;

INSERT INTO tblCSharp VALUES (1, '第一章', 11986);
INSERT INTO tblCSharp VALUES (6, '第六章', NULL)

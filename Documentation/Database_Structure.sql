/*
 Navicat Premium Data Transfer

 Source Server         : QAQ
 Source Server Type    : MariaDB
 Source Server Version : 100703
 Source Host           : localhost:3306
 Source Schema         : CommunityCenter

 Target Server Type    : MariaDB
 Target Server Version : 100703
 File Encoding         : 65001

 Date: 03/06/2022 14:15:54
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for Comment
-- ----------------------------
DROP TABLE IF EXISTS `Comment`;
CREATE TABLE `Comment`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `User_id` int(11) NOT NULL,
  `Admin_Id` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `UpdateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `Archive` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 41 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for Post
-- ----------------------------
DROP TABLE IF EXISTS `Post`;
CREATE TABLE `Post`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `User_Id` int(11) NOT NULL,
  `PrivacyType_Id` int(11) NOT NULL,
  `Admin_Id` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `UpdateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `Archive` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 17 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for Post_Comment_Relation
-- ----------------------------
DROP TABLE IF EXISTS `Post_Comment_Relation`;
CREATE TABLE `Post_Comment_Relation`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Post_Id` int(11) NOT NULL,
  `Parent_Comment_Id` int(11) NOT NULL,
  `Comment_Id` int(11) NOT NULL,
  `Admin_Id` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `UpdateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `Archive` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `Comment`(`Comment_Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 41 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for PrivacyType
-- ----------------------------
DROP TABLE IF EXISTS `PrivacyType`;
CREATE TABLE `PrivacyType`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Admin_Id` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `UpdateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `Archive` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `Name`(`Name`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for User
-- ----------------------------
DROP TABLE IF EXISTS `User`;
CREATE TABLE `User`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Account` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Password` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Admin_Id` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `UpdateTime` datetime NOT NULL DEFAULT utc_timestamp(),
  `Archive` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `Account`(`Account`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;

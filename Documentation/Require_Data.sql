SELECT A.`Id`
	,A.`Content`
	,A.`User_Id`
	,A.`PrivacyType_Id`
	,A.`Admin_Id`
	,A.`CreateTime`
	,A.`UpdateTime`
	,A.`Archive`
FROM `CommunityCenter`.`Post` AS A
LEFT JOIN `CommunityCenter`.`Post_Comment_Relation` AS B
ON A.`Id` = B.`Post_Id`
WHERE A.`PrivacyType_Id` = 1
AND B.`Archive` = 0
GROUP BY A.`Id`
ORDER BY COUNT(B.`Id`) DESC
LIMIT 10;
#  מנגנון בריחת אויבים מהשחקן

## תיאור הפרויקט
בפרויקט זה פותח מנגנון המאפשר לאויבים לברוח מהשחקן בצורה חכמה ויעילה. המנגנון מבוסס על חישוב מיקום רחוק מהשחקן במפת המשחק, תוך שימוש באלגוריתם מותאם. האויבים זזים בגריד מוגדר, שבו רק אריחים מסוימים נחשבים חוקיים לתנועה.

---

## תכונות עיקריות
- **בריחה חכמה:** האויבים בוחרים את האריח הרחוק ביותר האפשרי מהמיקום הנוכחי של השחקן.
- **תנועה דינמית וחלקה:** האויבים מתקדמים בכל פריים למיקום הבא תוך שמירה על מהירות קבועה.
- **מניעת בעיות בפינות:** המנגנון מתחשב בשכנים החוקיים כדי למנוע מצב שבו האויבים "נתקעים".
- **ניהול גריד דינמי:** השימוש במחלקת `GameWorld` לניהול האריחים והשכנים.

---

## מבנה המערכת

### 1. Escaper
מחלקה זו אחראית על מנגנון הבריחה:
- מחשבת את מיקום האויב והשחקן בגריד.
- בוחרת את האריח החוקי הרחוק ביותר מהמיקום הנוכחי של השחקן.
- מתקדמת למיקום הבא בצורה דינמית.

**דוגמת קוד:**
```csharp
using System.Collections.Generic;
using UnityEngine;

public class Escaper : MonoBehaviour {
    [SerializeField] private Transform player; 
    [SerializeField] private float moveSpeed = 2f;

    private void Update() {
        Vector3Int currentGridPosition = GameWorld.Instance.Tilemap.WorldToCell(transform.position);
        Vector3Int playerGridPosition = GameWorld.Instance.Tilemap.WorldToCell(player.position);

        List<Vector3Int> neighbors = GameWorld.Instance.GetNeighbors(currentGridPosition);

        Vector3Int farthestNode = currentGridPosition;
        float maxDistance = Vector3.Distance(currentGridPosition, playerGridPosition);

        foreach (var neighbor in neighbors) {
            float distance = Vector3.Distance(neighbor, playerGridPosition);
            if (distance > maxDistance) {
                maxDistance = distance;
                farthestNode = neighbor;
            }
        }

        Vector3 targetPosition = GameWorld.Instance.Tilemap.GetCellCenterWorld(farthestNode);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}

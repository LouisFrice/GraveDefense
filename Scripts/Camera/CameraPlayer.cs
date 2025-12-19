using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public Transform target; // 摄像机目标物体
    public Vector3 offsetPos; // 摄像机与目标物体的偏移位置
    public float bodyHeight; //看向目标物体的高度
    public float moveSpeed; // 摄像机移动速度
    public float rotateSpeed; // 摄像机旋转速度

    private Vector3 targetPos;
    private Vector3 lookPos;
    private Quaternion targetRotate;

    private bool isFirstView;
    private Transform firePoint;

    void Start()
    {
        firePoint = target.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/WeaponContainer/weapon_handgun/FirePoint");
    }

    
    void Update()
    {

        //没有目标直接返回
        if (target == null)
        {
            return;
        }

        if(Input.GetMouseButtonDown(1)) // 鼠标右键切换第一人称视角
        {
            isFirstView = !isFirstView;
        }
        //else if (Input.GetMouseButtonUp(1))
        //{
        //    isFirstView = false;
        //}

        //第一人称视角
        if (isFirstView)
        {

            //目标向后偏移Z轴
            targetPos = firePoint.position + firePoint.forward * -2;
            //目标向上偏移Y轴
            targetPos += Vector3.up * 0.5f; // 目标位置的y轴乘以bodyHeight
                                                   //目标向右偏移X轴
            targetPos += firePoint.right * -0.1f; // 目标位置的x轴乘以bodyHeight
                                                     //插值移动摄像机位置
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);
            //让摄像机看向目标物体
            lookPos = firePoint.position + Vector3.up * 0.1f; // 目标位置的y轴乘以bodyHeight
            targetRotate = Quaternion.LookRotation(lookPos - this.transform.position);
            
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotate, rotateSpeed * Time.deltaTime);
        }
        else
        {
            //目标位置+目标Z轴方向*Z轴偏移量=(x,y,z)+(0,0,1)*(0,0,-z)
            //目标向后偏移Z轴
            targetPos = target.position + target.forward * offsetPos.z;
            //目标向上偏移Y轴
            targetPos += Vector3.up * offsetPos.y; // 目标位置的y轴乘以bodyHeight
                                                   //目标向右偏移X轴
            targetPos += target.right * offsetPos.x; // 目标位置的x轴乘以bodyHeight
                                                     //插值移动摄像机位置
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);
            //让摄像机看向目标物体
            lookPos = target.position + Vector3.up * bodyHeight; // 目标位置的y轴乘以bodyHeight
            targetRotate = Quaternion.LookRotation(lookPos - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotate, rotateSpeed * Time.deltaTime);

        }
    }

    public void SetCameraTarget(Transform cameraTarget)
    {
        target = cameraTarget;
    }
}

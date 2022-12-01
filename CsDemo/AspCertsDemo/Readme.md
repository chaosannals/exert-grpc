# 

```bash
# 查看 Windows 证书
certmgr.msc
```

### 根证书生成

生成后 在 certmgr.msc 中间证书颁发机构   证书 

-DnsName 参数值必须与应用的部署目标匹配。 例如，用于开发的“localhost”。

```bash
# 生成证书，会打印出一个 Thumbprint 一串
New-SelfSignedCertificate -DnsName "localhost", "localhost" -CertStoreLocation "cert:\LocalMachine\My" -NotAfter (Get-Date).AddYears(20) -FriendlyName "localhost" -KeyUsageProperty All -KeyUsage CertSign, CRLSign, DigitalSignature

# 给密码加个密，密码是  "1234"
$mypwd = ConvertTo-SecureString -String "1234" -Force -AsPlainText

# 把 Thumbprint 替换为 New-SelfSignedCertificate 成功时候打印的内容。
# 得到私钥
Get-ChildItem -Path cert:\LocalMachine\My\Thumbprint | Export-PfxCertificate -FilePath C:\root_ca_dev_damienbod.pfx -Password $mypwd

# 把 Thumbprint 替换为 New-SelfSignedCertificate 成功时候打印的内容。
Export-Certificate -Cert cert:\localMachine\my\Thumbprint -FilePath root_ca_dev_damienbod.crt
```

### 从根证书创建子证书

```bash
# 把 Thumbprint 替换为 根证书的 Thumbprint 值
$rootcert = ( Get-ChildItem -Path cert:\LocalMachine\My\Thumbprint )

# 生成新证书，会打印出 Thumbprint 值
New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -DnsName "localhost" -Signer $rootcert -NotAfter (Get-Date).AddYears(20) -FriendlyName "localhost"

# 给密码加个密，密码是  "1234"
$mypwd = ConvertTo-SecureString -String "1234" -Force -AsPlainText

# 把 Thumbprint 替换为 新证书的 Thumbprint 值
Get-ChildItem -Path cert:\LocalMachine\My\Thumbprint | Export-PfxCertificate -FilePath C:\child_a_dev_damienbod.pfx -Password $mypwd

# 把 Thumbprint 替换为 新证书的 Thumbprint 值
Export-Certificate -Cert cert:\LocalMachine\My\Thumbprint -FilePath child_a_dev_damienbod.crt
```

根证书加入到 运行（mmc，[添加/删除管理单元]添加 [证书],“本地计算机” 的证书） [第三方根证书颁发机构]或[受信任的根证书颁发机构]（Trusted Root Certification Authorities） 证书，创建的子证书才能受信任。
因为 certmgr.msc 下只能添加当前用户只有浏览器起效，无法给服务器起效，所以要用 mmc。
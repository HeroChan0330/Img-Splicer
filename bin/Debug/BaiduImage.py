# -*- coding: utf8 -*-
import requests
import json
import re
import time

targetPath='raw/'

def Search(kw,offset,count=30):
    url='https://image.baidu.com/search/acjson?tn=resultjson_com&ipn=rj&fp=result&cl=&lm=&ie=utf-8&word=%s&pn=%s&rn=%s'
    requestRes=requests.get(url%(kw,offset,count),timeout=500,headers={'Host':'image.baidu.com','Referer':'https://image.baidu.com/','User-Agent':'Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.167 Safari/537.36'})
    requestRes.encoding='utf-8'
    #print requestRes.text
    if requestRes.status_code==200:
        return json.loads(requestRes.content,encoding='utf-8')


def DownLoad(url):
    global targetPath
    try:
        fileName=url[url.rfind('/')+1:]
        res=requests.get(url,timeout=100,headers={'Referer':'https://image.baidu.com/','User-Agent':'Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.167 Safari/537.36'})
        if res.status_code==200:
            with open(targetPath+fileName,'wb') as fp:
                fp.write(res.content)
                fp.close()
    except:
        print 'fail to download:'+url

def DownLoadPreViewImg(res):
    index=0
    for item in res['data']:
        if 'middleURL' in item:
            print str(index)+ ' ' +item['middleURL']
            DownLoad(item['middleURL'])
        index+=1

# def DownLoadPrimaryImg(res):
#     index=0
#     for item in res['data']:
#         if 'replaceUrl' in item:
#             url=item['replaceUrl'][1]['ObjURL']
#             print str(index)+ ' ' +url
#             DownLoad(url)
#         index+=1

keyWord='小猪佩奇'

for i in range(0,50):
    try:
        res=Search(keyWord,i*30,30)
        DownLoadPreViewImg(res)
        sleep(1)
    except:
        print 'fail'
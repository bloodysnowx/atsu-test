'///////////////////////////////////////////////////
' URLとタイトルのコピー
' by ケヴィン
'　↑をLatesが改造してPTR用サマリをクリップボーへ送るにしました。
'///////////////////////////////////////////////////
Option Explicit

    Dim obj, document, window,id
    Set obj = CreateObject("Sleipnir.API")
    id = obj.GetDocumentID(obj.ActiveIndex)
    
    Set document = obj.GetDocumentObject(id)
    Set window = obj.GetWindowObject(id)
    
    dim ptrhtml
    dim Earn, BB100, Hands, Rate
    dim strDate

    ptrhtml = Document.Body.InnerHtml
    Rate = getRate(ptrhtml)
    'ptrhtmlを処理用に分割
    ptrhtml = Right(ptrhtml, Len(ptrhtml) - InStr(ptrhtml, "TBODY"))

    BB100 = getExBB(ptrhtml)
    
    strDate = makedate()
    
    dim letstr
    letstr = "R"+Rate
    letstr = letstr & "  H " & Hands
    letstr = letstr & "  $ " & Earn
    letstr = letstr & "  BB " & BB100
    letstr = letstr & "  " & strDate

    If document Is Nothing Then
        Call obj.MessageBox("Document を作成できません")
    Else
        window.clipboardData.setData "text", letstr
    
        Set document = Nothing
        Set window = Nothing
    End If
    
    Set obj = Nothing
'------------------------------------------------------------
Function makeDate()

    dim strDate
    dim strMonth
    dim strDay
    
    strDate = Year(Date)
    
    If Month(Date) <10 then
        
        strMonth= 0 & Month(Date)
    else
        strMonth= Month(Date)
    
    End if

    IF Day(Date)  <10 then
    
        strDay = 0 & Day(Date)
    else
        strDay = Day(Date)
    End if

    
    makeDate = strDate & "/" & strMonth & "/" & strDay
    
End Function
'------------------------------------------------------------
Function getRate(strhtml)
    
    Dim objRE, oMatche, oMatches, RetStr, mystr, objRE2
    Set objRE = new RegExp
    objRE.Global = True 
    objRE.IgnoreCase = True
    objRE.pattern = "<div class=ov_main_rate.*?>.*?</div>"

    set oMatches = objRE.execute(strhtml)
    set oMatche= oMatches(0)
    mystr= oMatche.value
    
    mystr = Replace(mystr,"</DIV>","")
    
    set objRE2 = new RegExp
    objRE2.Ignorecase =True
    objRE2.pattern ="<div class=ov_main_rate.*?>"
    
    mystr = objRE2.Replace(mystr,"")
    
    getRate = mystr

End Function
'------------------------------------------------------------
Function getExBB(strhtml)

    Dim objRE, i, BB_sum
    Dim m_stake, m_Hands, m_BB, m_Earn
    Set objRE = new RegExp
    objRE.Global = True 
    objRE.IgnoreCase = True
    
    objRE.pattern = "<TD\swidth=150.*?><A\shref=.*?>(.*?)</A></TD>"
    Set m_stake = objRE.execute(strhtml)

    objRE.pattern = "<TD>([0-9|,]+)</TD>"
    Set m_Hands = objRE.execute(strhtml)

    'objRE.pattern = "<TD>(<SPAN class=(positive|negative)>|)(?<BB100>(-|)([0-9|,]+\.[0-9]+)*?)(</SPAN>|)</TD>"
    objRE.pattern = ">(-?[0-9|,]+\.[0-9]+)<"
    Set m_BB = objRE.execute(strhtml)

    objRE.pattern = ">\$(-?[0-9|,]+)<"
    Set m_Earn = objRE.execute(strhtml)

    BB_sum = 0
    Earn = 0
    Hands = 0

    'Call obj.MessageBox("m_stake length = " & m_stake.Count)
    'Call obj.MessageBox("m_Hands length = " & m_Hands.Count)
    'Call obj.MessageBox("m_BB length = " & m_BB.Count)

    For i = 0 To m_stake.Count - 1
        If is10maxNLabove(m_stake(i).SubMatches(0)) Then
            hands = hands + m_Hands(i).SubMatches(0)
            BB_sum = BB_sum + m_Hands(i).SubMatches(0) * m_BB(i).SubMatches(0)
            Earn = Earn + m_Earn(i).SubMatches(0)
            'Call obj.MessageBox("stake = " & m_stake(i).SubMatches(0) & " hands = " & m_Hands(i).SubMatches(0) & " bb = " & m_BB(i).SubMatches(0) & "Earn = " & m_Earn(i).SubMatches(0))
        End If
    Next

    getExBB = Round(BB_sum / hands, 2)

End Function
'------------------------------------------------------------
Function is10maxNLabove(stakesName)

    is10maxNLabove = true

    If InStr(stakesName, "NLH") < 1 Then
        is10maxNLabove = false 'NLH以外は除外
    End If

    If InStr(stakesName, "HU") > 1 Then
        is10maxNLabove = false 'HUは除外
    End If
    
    If InStr(stakesName, "0.02") > 1 Or InStr(stakesName, "0.05") > 1 Then
        is10maxNLabove = false '0.01/0.02, 0.02/0.05, 0.05/0.1 を除外
    End If

End Function

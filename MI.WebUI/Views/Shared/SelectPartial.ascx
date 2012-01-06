<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<select>
    <option value=''></option>
    <% foreach (KeyValuePair<int, string> value in (Dictionary<int, string>)Model)
       { %><option
        value='<%= Html.Encode(value.Key.ToString())%>'>
        <%= Html.Encode(value.Value)%></option>
    <% } %>
</select>

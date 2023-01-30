function changeColor(colorParam) {
  let color = colorParam.value.toLowerCase();
  var optionElement = document.getElementById(colorParam);
  optionElement.style.color = color;
}

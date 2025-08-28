import { phonecall } from "react-native-communications";

const namespace = "https://hypermedia.systems/hyperview/communications";

export const Phone = {
  action: "open-phone",
  callback: (element: Element) => {
    const number = element.getAttributeNS(namespace, "phone-number");
    if (number != null) {
      phonecall(number, false);
    }
  },
};

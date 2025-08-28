import type { HvBehavior } from "hyperview";
import { email } from "react-native-communications";

const namespace = "https://hypermedia.systems/hyperview/communications";

export const Email: HvBehavior = {
  action: "open-email",
  callback: (element: Element) => {
    const emailAddress = element.getAttributeNS(namespace, "email-address");
    if (emailAddress != null) {
      email([emailAddress], [""], [""], "", "");
    }
  },
};

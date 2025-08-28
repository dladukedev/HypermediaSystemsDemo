import RootToast from "react-native-root-toast";

const namespace = "https://hypermedia.systems/hyperview/message";

export const Toast = {
  action: "show-message",
  callback: (element: Element) => {
    const text = element.getAttributeNS(namespace, "text");
    if (text != null) {
      RootToast.show(text, {
        position: RootToast.positions.TOP,
        duration: 2000,
      });
    }
  },
};
